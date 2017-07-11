using CSCLI.Models;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSCLI
{
    public static class Scripting
    {
        private static Script CurrentScript { get; set; }
        private static ScriptState LastState { get; set; }

        private static string ArrayOutput { get; set; } = "";

        public static void NewScript()
        {
            var usings = "";
            foreach (var line in Settings.Current.Usings)
            {
                usings += $"using {line};{Environment.NewLine}";
            }
            CurrentScript = CSharpScript.Create(usings, ScriptOptions.Default.AddReferences(Settings.Current.References));
            LastState = null;
        }
        public static async Task<string> RunScript(string Input)
        {
            try
            {
                var newScript = CurrentScript.ContinueWith(Input);
                if (LastState == null)
                {
                    LastState = await newScript.RunAsync(null, CancellationToken.None);
                }
                else
                {
                    LastState = await newScript.RunFromAsync(LastState, CancellationToken.None);
                }
                if (LastState.ReturnValue == null)
                {
                    CurrentScript = newScript;
                    return "[No output]" + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    CurrentScript = newScript;
                    if (LastState.ReturnValue is IEnumerable)
                    {
                        ArrayOutput = "";
                        BuildArrayOutput(LastState.ReturnValue as IEnumerable, 0);
                        return ArrayOutput;
                    }
                    else
                    {
                        return LastState.ReturnValue.ToString() + Environment.NewLine + Environment.NewLine;
                    }
                }
            }
            catch (CompilationErrorException ex)
            {
                return $"Error: {ex.Message}" + Environment.NewLine + Environment.NewLine;
            }
        }
        private static void BuildArrayOutput(IEnumerable ArrayValue, int Indents)
        {
            for (var i = 0; i < Indents; i++)
            {
                ArrayOutput += "    ";
            }
            ArrayOutput += ArrayValue.ToString() + Environment.NewLine;
            foreach (var value in ArrayValue)
            {
                if (value is IEnumerable && value is String == false)
                {
                    BuildArrayOutput(value as IEnumerable, Indents + 1);
                }
                else
                {
                    for (var i = 0; i < Indents + 1; i++)
                    {
                        ArrayOutput += "    ";
                    }
                    ArrayOutput += value.ToString() + $" [{value.GetType().ToString()}]{Environment.NewLine}";
                }
            }
        }
    }
}
