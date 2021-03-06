﻿namespace ConsoLovers.UnitTests.ConsoleApplicationWithTests.Utils
{
   using System;

   using ConsoLovers.ConsoleToolkit;
   using ConsoLovers.ConsoleToolkit.CommandLineArguments;

   using JetBrains.Annotations;

   public class TestApplication<T> : ConsoleApplicationWith<T>
      where T : class
   {
      private readonly IApplicationVerification application;

      public override void RunWith(T arguments)
      {
         application.RunWith();
      }

      public override void InitializeArguments(T instance, string[] args)
      {
         base.InitializeArguments(instance, args);

         var info = new ArgumentClassInfo(typeof(T));
         foreach (var property in info.Properties)
            application.Argument(property.ParameterName, property.PropertyInfo.GetValue(instance));
      }

      public override void Run()
      {
         application.Run();
         base.Run();
      }

      public override void RunWithCommand(ICommand command)
      {
         application.RunWithCommand();
         base.RunWithCommand(command);
      }

      protected override void RunWithoutArguments()
      {
         application.RunWithoutArguments();
         base.RunWithoutArguments();
      }

      public TestApplication([NotNull] ICommandLineEngine commandLineEngine, [NotNull] IApplicationVerification application)
         : base(commandLineEngine)
      {
         if (application == null)
            throw new ArgumentNullException(nameof(application));

         this.application = application;
      }
   }
}