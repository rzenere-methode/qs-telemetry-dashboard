﻿using System.Collections.Generic;
using System.Linq;

using qs_telemetry_dashboard.Exceptions;

namespace qs_telemetry_dashboard
{
	internal class ArgumentManager
	{
		internal bool NoArgs { get; }
		internal bool TaskTriggered { get; }
		internal bool DebugLog { get; }
		internal bool TestRun { get; }
		internal bool InitializeRun { get; }
		internal bool FetchMetadataRun { get; }

		internal const string HELP_STRING =
@"Telemetry Dashboard

Arguments:
-debug				activate debug logging
-initialize			initialize the environment with app, tasks and data connections
-fetchmetadata		generated metadata files to Telemetry Dashboard output folder
-test				test to make sure certificates are in place and the QRS is running";

		public ArgumentManager(string[] args, bool isComandLineRun)
		{
			TaskTriggered = false;
			NoArgs = false;
			DebugLog = false;
			TestRun = false;
			InitializeRun = false;
			FetchMetadataRun = false;

			if (args.Length == 0)
			{
				if (isComandLineRun)
				{
					NoArgs = true;
				}
				else
				{
					InitializeRun = true;
				}
			}
			else
			{
				Dictionary<string, string> argDic = args.ToDictionary(
					x =>
					{
						if (x.Contains('='))
						{
							return x.Split('=')[0].Substring(1);
						}
						else
						{
							return x;
						}
					},
					y =>
					{
						if (y.Contains('='))
						{
							return y.Split('=')[1];
						}
						else
						{
							return "";
						}
					});

				string argValue = "";

				if (argDic.TryGetValue("-fetchmetadata", out argValue))
				{
					argDic.Remove("-fetchmetadata");
					FetchMetadataRun = true;
				}

				if (argDic.TryGetValue("-tasktriggered", out argValue))
				{
					argDic.Remove("-tasktriggered");
					TaskTriggered = true;
				}

				if (argDic.TryGetValue("-debug", out argValue))
				{
					argDic.Remove("-debug");
					DebugLog = true;
				}

				if (argDic.TryGetValue("-test", out argValue))
				{
					argDic.Remove("-test");
					TestRun = true;
					DebugLog = true;
				}
				if (argDic.TryGetValue("-initialize", out argValue))
				{
					argDic.Remove("-initialize");
					InitializeRun = true;
				}


				if (argDic.Count > 0)
				{
					throw new ArgumentManagerException("Unhandled argument: " + string.Join(",", argDic.Select(kv => kv.Key).ToArray()));
				}
			}
		}
	}
}
