using CSCLI.Models;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
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
                    return LastState.ReturnValue.ToString() + Environment.NewLine + Environment.NewLine;
                }
            }
            catch (CompilationErrorException ex)
            {
                return $"Error: {ex.Message}" + Environment.NewLine + Environment.NewLine;
            }
        }
    }
}
