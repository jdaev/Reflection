using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(Command))]
    public class CommandPropertyDrawer : PropertyDrawer
    {
        private MethodInfo[] availableMethods;
        private List<Command> availableCommands = new List<Command>();
        int _choiceIndex = 0;
        private Dictionary<string, MethodInfo> methodDict = new Dictionary<string, MethodInfo>();
        private bool initialized = false;
        private Vector2 _vector2Val;
        private int _intVal;

        public void Initialize()
        {
            availableMethods = typeof(AIPlayer).GetMethods().Where((e) =>
            {
                return e.GetCustomAttribute(typeof(AIPlayerTargetAttribute)) != null;
            }).Cast<MethodInfo>().ToArray();
            foreach (MethodInfo methodInfo in availableMethods)
            {
                Type methodArgumentType = methodInfo.GetParameters().FirstOrDefault()?.ParameterType;

                availableCommands.Add(new Command(methodInfo.Name,
                    methodArgumentType != null ? System.Activator.CreateInstance(methodArgumentType) : null));


                methodDict.Add(methodInfo.Name, methodInfo);
            }

            initialized = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!initialized) Initialize();
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 2;
            SerializedProperty argument = property.FindPropertyRelative("argument");
            SerializedProperty commandName = property.FindPropertyRelative("commandName");
            Command command = property.GetSerializedValue<Command>();

            var targetObject = property.serializedObject.targetObject;
            var targetObjectClassType = targetObject.GetType();
            AIPlayer aiPlayer = (AIPlayer) targetObject;
            
            // Command command = new Command("Dummy", 0);
            // //Command command = aiPlayer.commandsToExecute[];
            // var field = targetObjectClassType.GetField(property.propertyPath);
            //
            // if (field != null)
            // {
            //     var value = field.GetValue(targetObject);
            // }

            Rect labelRect = new Rect(position.x, position.y, 200, position.height);
            Rect valueRect = new Rect(position.x + 210, position.y, 100, position.height);


            _choiceIndex = EditorGUI.Popup(labelRect, _choiceIndex, methodDict.Keys.ToArray());
            String methodName = methodDict.Keys.ToArray()[_choiceIndex];
            Type methodArgumentType = methodDict[methodName].GetParameters()
                .FirstOrDefault()?.ParameterType;

            command.commandName = methodName;

            if (methodArgumentType == typeof(Vector2))
                command.argument = _vector2Val = EditorGUI.Vector2Field(valueRect, "", _vector2Val);
            else if (methodArgumentType == typeof(int))
                command.argument = _intVal = EditorGUI.IntField(valueRect, _intVal);

            EditorGUI.indentLevel = indent;


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}