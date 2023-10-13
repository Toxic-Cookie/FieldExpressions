using System;
using CodingSeb.ExpressionEvaluator;
using HarmonyLib;
using ResoniteModLoader;
using Elements.Core;
using FrooxEngine;

namespace FieldExpressions.Core
{
    class FieldExpressions : ResoniteMod
    {
        public override string Name => "FieldExpressions";
        public override string Author => "Toxic_Cookie";
        public override string Version => "1.0.4";
        public override string Link => "https://github.com/Toxic-Cookie/FieldExpressions";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.Toxic_Cookie.FieldExpressions");
            harmony.PatchAll();

            evaluator.OptionCaseSensitiveEvaluationActive = false;
            evaluator.OptionInlineNamespacesEvaluationRule = InlineNamespacesEvaluationRule.AllowOnlyInlineNamespacesList;
        }

        public static ExpressionEvaluator evaluator = new ExpressionEvaluator();

        [HarmonyPatch(typeof(PrimitiveMemberEditor), "ParseAndAssign")]
        class AllowExpressionsInFields
        {
            static bool Prefix(PrimitiveMemberEditor __instance, ref SyncRef<TextEditor> ____textEditor, ref FieldDrive<string> ____textDrive)
            {
                try
                {
                    StructMemberAccessor structFieldAccessor = __instance.Accessor;

                    if (structFieldAccessor != null)
                    {
                        try
                        {
                            if (structFieldAccessor.TargetType != typeof(string))
                            {
                                if (PrimitiveTryParsers.GetParser(structFieldAccessor.TargetType)(evaluator.Evaluate(____textEditor.Target.Text.Target.Text.Replace(',', '.')).ToString(), out var _parsed))
                                {
                                    __instance.SetMemberValue(_parsed);
                                }
                            }
                            else
                            {
                                if (PrimitiveTryParsers.GetParser(structFieldAccessor.TargetType)(____textEditor.Target.Text.Target.Text, out var parsed))
                                {
                                    __instance.SetMemberValue(parsed);
                                }
                            }
                        }
                        catch
                        {
                            if (PrimitiveTryParsers.GetParser(structFieldAccessor.TargetType)(____textEditor.Target.Text.Target.Text, out var parsed))
                            {
                                __instance.SetMemberValue(parsed);
                            }
                        }
                    }
                }
                catch (Exception arg)
                {
                    UniLog.Error($"Exception assigning value to {____textDrive.Target}:\n{arg}");
                }

                return false;
            }
        }
    }
}
