namespace ConsoLovers.UnitTests.ArgumentEngine
{
   using ConsoLovers.ConsoleToolkit.CommandLineArguments;

   internal class ApplicationArgs
   {
      #region Public Properties

      [Command("execute")]
      public GenericCommand<ExecuteArgs> Execute { get; set; }

      #endregion
   }
}