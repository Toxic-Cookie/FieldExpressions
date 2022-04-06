# FieldExpressions

There is currently a RCE exploit due to the lack of sandboxing in codingseb's expression evaluator. Please do not install this mod until I replace the library with one that is properly sandboxed.

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that allows you to use [CodingSeb's Expression Evaluator](https://github.com/codingseb/ExpressionEvaluator/wiki/Getting-Started) in inspector fields.

## Installation
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
2. Place [FieldExpressions.dll](https://github.com/Toxic-Cookie/FieldExpressions/releases) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
3. Start the game. If you want to verify that the mod is working you can check your Neos logs.