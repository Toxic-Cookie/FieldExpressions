using System;
using BaseX;
using FrooxEngine;
using HarmonyLib;
using NCalc;
using NeosModLoader;

namespace FieldExpressions
{
	class FieldExpressions : NeosMod
	{
		public override string Name => "FieldExpressions";
		public override string Author => "Toxic_Cookie";
		public override string Version => "1.0.2";
		public override string Link => "https://github.com/Toxic-Cookie/FieldExpressions";

		public override void OnEngineInit()
		{
			Harmony harmony = new Harmony("net.Toxic_Cookie.FieldExpressions");
			harmony.PatchAll();

			Expression.CacheEnabled = false;
		}

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
								Expression e = new Expression(____textEditor.Target.Text.Target.Text.Trim('\r', '\n'), EvaluateOptions.IgnoreCase);
								if (PrimitiveTryParsers.GetParser(structFieldAccessor.TargetType)(e.Evaluate().ToString(), out var _parsed))
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
						catch (Exception ex)
						{
							Debug(ex);
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
