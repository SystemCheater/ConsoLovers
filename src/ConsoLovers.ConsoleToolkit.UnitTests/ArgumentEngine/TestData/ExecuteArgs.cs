// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteArgs.cs" company="ConsoLovers">
//    Copyright (c) ConsoLovers  2015 - 2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoLovers.UnitTests.ArgumentEngine.TestData
{
   using ConsoLovers.ConsoleToolkit.CommandLineArguments;

   internal class ExecuteArgs
   {
      #region Public Properties

      [Argument("Path")]
      [HelpText("PathHelp")]
      public string Path { get; set; }

      [Option("silent")]
      [HelpText("SilentHelp")]
      public bool Silent { get; set; }

      #endregion
   }
}