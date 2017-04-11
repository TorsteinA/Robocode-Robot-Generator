﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp;

namespace GeneticAlgorithmForStrings
{
	internal class DLLFileCreator
	{
		internal static void CreateDll(string dllDirPath, int generation, int individual) {

			var classCodeAsString = RobotFileCreator.GetFileText(generation, individual);

			var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
			var parameters =
				new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll", "Robocode.dll" },
					$"{dllDirPath}/{FileCreator.GetRobotName(generation, individual)}.dll", true) 
				{
					GenerateExecutable = false,
					//TODO: get write access to subdirs
					//			            OutputAssembly = _directoryPath
				};
			var results = csc.CompileAssemblyFromSource(parameters,
				classCodeAsString);
			results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));

		}
	}
}