using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodingSeb.ExpressionEvaluator;
using HarmonyLib;
using NeosModLoader;
using BaseX;
using FrooxEngine;

namespace FieldExpressions.Core
{
    class FieldExpressions : NeosMod
    {
        public override string Name => "FieldExpressions";
        public override string Author => "Toxic_Cookie";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/Toxic-Cookie/FieldExpressions";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.Toxic_Cookie.FieldExpressions");
            harmony.PatchAll();

            evaluator.OptionCaseSensitiveEvaluationActive = false;
        }

        public static ExpressionEvaluator evaluator = new ExpressionEvaluator();

        [HarmonyPatch(typeof(PrimitiveMemberEditor), "ParseAndAssign")]
        class AllowExpressionsInFields
        {
            static bool Prefix(PrimitiveMemberEditor __instance, ref SyncRef<TextEditor> ____textEditor, ref FieldDrive<string> ____textDrive)
            {
                try
                {
                    StructFieldAccessor structFieldAccessor = __instance.Accessor;

                    if (structFieldAccessor != null)
                    {
                        try
                        {
                            if (structFieldAccessor.TargetType != typeof(string))
                            {
                                if (PrimitiveTryParsers.GetParser(structFieldAccessor.TargetType)(evaluator.ScriptEvaluate(____textEditor.Target.Text.Target.Text + ";").ToString(), out var _parsed))
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
