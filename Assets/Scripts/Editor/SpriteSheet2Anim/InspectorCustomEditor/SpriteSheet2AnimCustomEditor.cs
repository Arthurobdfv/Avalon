using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(SpriteSheet2AnimDefinition))]
[Serializable]
public class SpriteSheet2AnimCustomEditor : Editor
{
    public List<string> CurrentFilesToParse = new List<string>();

    public static Dictionary<string, SpriteSheetDefinition> SpriteSheetDefinitionsLookup = new();

    string basePath = Application.dataPath;
    string FullPath => Path.GetFullPath(Path.Combine(basePath, Target.CharacterAssetDefinition?.CharacterAssetPath));

    SpriteSheet2AnimDefinition Target => target as SpriteSheet2AnimDefinition;
    void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        if (Target == null)
        {
            throw new System.Exception("The Target is null!");
        }

        base.OnInspectorGUI();
        var imagesToParse = new List<string>();

        if (GUILayout.Button("Find Assets"))
        {
            imagesToParse = FindSourceImageFiles(FullPath).ToList();
        }
        if (GUILayout.Button("Parse Sprites"))
        {
            if (imagesToParse.Count == 0)
            {
                imagesToParse = FindSourceImageFiles(FullPath).ToList();
            }
            SpriteSheetDefinitionsLookup = imagesToParse.Select(x => new { filePath = Path.GetFileName(x), Target.SpriteSheet }).ToDictionary(x => x.filePath, y => y.SpriteSheet);

            foreach (var item in SpriteSheetDefinitionsLookup)
            {
                ParseImageAt($"Assets/{Target.CharacterAssetDefinition?.CharacterAssetPath}/{Path.GetFileName(item.Key)}");
            }
            SpriteSheetDefinitionsLookup.Clear();
        }

        if (GUILayout.Button("Generate Animations"))
        {
            // TODO: Support multi-spritesheet animations
            var allSprites = AssetDatabase.FindAssets("t:Sprite", new[] { $"Assets/{Target.CharacterAssetDefinition?.CharacterAssetPath}" }).Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Texture2D>)
                .ToList();
            allSprites.ForEach(x => AnimationsFromTexture(x, Target.SpriteSheetAnimationDescriptors));
        }
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }

    private static string[] FindSourceImageFiles(string path)
    {
        if (Directory.Exists(path))
        {
            var imgFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
            Debug.Log($"Found {imgFiles.Length} files:{string.Join($"{Environment.NewLine} * ", imgFiles)}");
            return imgFiles;
        }
        else
        {
            Debug.LogError($"Path not found: {path}");
            return Array.Empty<string>();
        }
    }

    private void ParseImageAt(string path)
    {
        AssetDatabase.ImportAsset(path, ImportAssetOptions.Default);
    }

    private void AnimationsFromTexture(Texture2D tex, List<SpriteSheetAnimationDescriptor> spriteSheetAnimationDescriptors)
    {
        var assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(tex)).Select(x => x as Sprite).Where(x => x != null).ToList();
        if (!Directory.Exists(Path.Combine($"{Application.dataPath}", Target.CharacterName)))
        {
            AssetDatabase.CreateFolder("Assets", $"{Target.CharacterName}");
        }
        var animationList = new List<AnimationClip>();
        var controller = GetOrCreateAnimatorForCharacter();
        foreach (var animationDescrition in spriteSheetAnimationDescriptors)
        {
            Debug.Log($"Creating animation for {animationDescrition.AnimationName} from asset {animationDescrition.AnimationSetDefinition.name}");
            var animationDirectionMapping = new Dictionary<string, AnimationClip>();
            var startIndex = animationDescrition.StartIndex;
            int currentIndex = (int)startIndex;
            foreach (var direction in animationDescrition.AnimationSetDefinition.Directions)
            {
                var spriteCount = animationDescrition.AnimationSetDefinition.SpritePerAnimation;
                var animation = new AnimationClip();
                var sett = AnimationUtility.GetAnimationClipSettings(animation);
                sett.loopTime = true;

                AnimationUtility.SetAnimationClipSettings(animation, sett);
                EditorCurveBinding spriteBinding = new EditorCurveBinding();
                spriteBinding.type = typeof(SpriteRenderer);
                spriteBinding.path = "";
                spriteBinding.propertyName = "m_Sprite";

                ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[spriteCount];
                for (int i = currentIndex; i < currentIndex + spriteCount; i++)
                {
                    var current = i - currentIndex;
                    spriteKeyFrames[current] = new ObjectReferenceKeyframe
                    {
                        time = current,
                        value = assets[i]
                    };
                }
                currentIndex += spriteCount;
                animation.frameRate = 1f;
                AnimationUtility.SetObjectReferenceCurve(animation, spriteBinding, spriteKeyFrames);
                if (!Directory.Exists(Path.Combine($"{Application.dataPath}", $"{Target.CharacterName}/{animationDescrition.AnimationName}")))
                {
                    AssetDatabase.CreateFolder($"Assets/{Target.CharacterName}", animationDescrition.AnimationName);
                }
                AssetDatabase.CreateAsset(animation, $"Assets/{Target.CharacterName}/{animationDescrition.AnimationName}/{animationDescrition.AnimationName}_{direction}.anim");
                animationDirectionMapping.Add(direction, animation);
                animationList.Add(animation);
            }
            if (animationDescrition.AnimationSetDefinition.DirectionCount > 1)
            {
                // TODO: Might move this logic somewhere else as in the future we could have multiple types of blend trees
                AssignAnimationsToBlendTree(animationDescrition.AnimationName, animationDirectionMapping, controller);
            }
        }
        CreateAvalonAnimatorTransitions(controller);
        Debug.Log($"Created {animationList.Count} animations;");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private AnimatorController GetOrCreateAnimatorForCharacter()
    {
        var t = AssetDatabase.FindAssets($"t:AnimatorController").Select(x => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(x), typeof(AnimatorController)) as AnimatorController).FirstOrDefault(x => x.name == Target.CharacterName);
        if (t != null)
        {
            return t;
        }
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath($"Assets/{Target.CharacterName}/{Target.CharacterName}.controller");
        controller.AddParameter("DirectionX", AnimatorControllerParameterType.Float);
        controller.AddParameter("DirectionY", AnimatorControllerParameterType.Float);
        controller.AddParameter("Movement", AnimatorControllerParameterType.Int);
        return controller;
    }

    private void AssignAnimationsToBlendTree(string blendTreeName, Dictionary<string, AnimationClip> animations, AnimatorController controller)
    {
        var blendTree = new BlendTree();
        var statess = controller.layers.Select(x => x.stateMachine).SelectMany(x => x.states).Where(x => x.state != null).Select(x => x.state);
        if(statess.Any(x => x.name == blendTreeName))
        {
            return;
        }

        var state = controller.CreateBlendTreeInController(blendTreeName, out blendTree, 0);
        // TODO: This can be a bledtree Setting asset
        state.speed = 16f;
        blendTree.name = blendTreeName;
        blendTree.blendType = BlendTreeType.SimpleDirectional2D;
        blendTree.blendParameter = "DirectionX";
        blendTree.blendParameterY = "DirectionY";
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        var position = (Quaternion.AngleAxis(0, Vector3.back)) * Vector2.up;
        var angleDiff = 360 / animations.Count();
        var eightDirectionalWheel = new Dictionary<string, Vector2>
        {
            { DirectionEnum.UP.ToString(), position },
            { DirectionEnum.UPPER_RIGHT.ToString(), position = (Quaternion.AngleAxis(angleDiff, Vector3.back)) * position },
            { DirectionEnum.RIGHT.ToString(), position = (Quaternion.AngleAxis(angleDiff, Vector3.back)) * position },
            { DirectionEnum.LOWER_RIGHT.ToString(), position = (Quaternion.AngleAxis(angleDiff, Vector3.back)) * position },
            { DirectionEnum.DOWN.ToString(), position = (Quaternion.AngleAxis(angleDiff, Vector3.back)) * position },
            { DirectionEnum.LOWER_LEFT.ToString(), position = (Quaternion.AngleAxis(angleDiff, Vector3.back)) * position },
            { DirectionEnum.LEFT.ToString(), position =(Quaternion.AngleAxis(angleDiff, Vector3.back)) * position },
            { DirectionEnum.UPPER_LEFT.ToString(), position =(Quaternion.AngleAxis(angleDiff, Vector3.back)) * position }
        };
        foreach (var animation in animations)
        {
            blendTree.AddChild(animation.Value, eightDirectionalWheel[animation.Key]);
        }

    }

    // TODO: Currntly tied up with Avalon specific animation transitions, the idea is turn this into an extensible system
    private void CreateAvalonAnimatorTransitions(AnimatorController controller)
    {
        if (controller.parameters.FirstOrDefault(x => x.name == "Movement") == null)
        {
            controller.AddParameter("Movement", AnimatorControllerParameterType.Int);
        }
        var states = controller.layers[0].stateMachine.states;
        var walkAnim = controller.layers[0].stateMachine.states.FirstOrDefault(x => x.state.name == "Walk").state;
        var idleAnim = controller.layers[0].stateMachine.states.FirstOrDefault(x => x.state.name == "Idle").state;
        var runningAnim = controller.layers[0].stateMachine.states.FirstOrDefault(x => x.state.name == "Running").state;


        // Walk Transitions
        var walkTransitions = walkAnim.transitions;
        var runningTransitions = runningAnim.transitions;
        var idleTransitions = idleAnim.transitions;

        controller.layers[0].stateMachine.defaultState = idleAnim;

        if(!walkTransitions.Any(x => x.destinationState) == runningAnim)
        {
            var walk2Run = walkAnim?.AddTransition(runningAnim, false);
            walk2Run?.AddCondition(AnimatorConditionMode.Equals, 2, "Movement");
        }
        if(!runningTransitions.Any(x => x.destinationState) == walkAnim)
        {
            var run2Walk = runningAnim?.AddTransition(walkAnim, false);
            run2Walk?.AddCondition(AnimatorConditionMode.Less, 2, "Movement");
        }

        // Idle Transitions
        if(!idleTransitions.Any(x => x.destinationState) == walkAnim)
        {
            var idle2Walk = idleAnim?.AddTransition(walkAnim, false);
            idle2Walk?.AddCondition(AnimatorConditionMode.Greater, 0, "Movement");
        }
        if(!walkTransitions.Any(x => x.destinationState) == idleAnim)
        {
            var walk2Idle = walkAnim?.AddTransition(idleAnim, false);
            walk2Idle?.AddCondition(AnimatorConditionMode.Equals, 0, "Movement");
        }
    }
}
