﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleApplicationWith.cs" company="ConsoLovers">
//    Copyright (c) ConsoLovers  2015 - 2017
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ConsoLovers.ConsoleToolkit
{
   using System;
   using System.Linq;

   using ConsoLovers.ConsoleToolkit.CommandLineArguments;
   using ConsoLovers.ConsoleToolkit.Contracts;
   using ConsoLovers.ConsoleToolkit.DIContainer;

   using JetBrains.Annotations;

   /// <summary>Base class for console applications with command line parameters</summary>
   /// <typeparam name="T">The type of the parameter class</typeparam>
   /// <seealso cref="ConsoLovers.ConsoleToolkit.IApplication{T}"/>
   /// <seealso cref="ConsoLovers.ConsoleToolkit.IArgumentInitializer{T}"/>
   /// <seealso cref="ConsoLovers.ConsoleToolkit.IExeptionHandler"/>
   public abstract class ConsoleApplicationWith<T> : IApplication<T>, IArgumentInitializer<T>, IExeptionHandler
      where T : class

   {
      #region Constants and Fields

      #endregion

      #region Constructors and Destructors

      [InjectionConstructor]
      protected ConsoleApplicationWith([NotNull] ICommandLineEngine commandLineEngine)
      {
         if (commandLineEngine == null)
            throw new ArgumentNullException(nameof(commandLineEngine));

         CommandLineEngine = commandLineEngine;
      }

      #endregion

      #region IApplication Members

      public virtual void Run()
      {
         var commandExecuted = ExecuteCommand(true);
         if (!commandExecuted)
         {
            if (HasArguments)
            {
               RunWith(Arguments);
            }
            else
            {
               RunWithoutArguments();
            }
         }
      }

      #endregion

      #region IApplication<T> Members

      public abstract void RunWith(T arguments);

      #endregion

      #region IArgumentInitializer<T> Members

      public virtual T CreateArguments()
      {
         if (GetType() == typeof(T))
            return this as T;

         return Activator.CreateInstance<T>();
      }

      public virtual void InitializeArguments(T instance, string[] args)
      {
         Args = args;
         HasArguments = args != null && args.Length > 0;
         Arguments = CommandLineEngine.Map(args, instance);

         OnArgumentsInitialized();
      }

      #endregion

      #region IExeptionHandler Members

      public virtual bool HandleException(Exception exception)
      {
         var missingArgumentException = exception as MissingCommandLineArgumentException;
         if (missingArgumentException != null)
         {
            Console.WriteLine("Invalid command line arguments", ConsoleColor.Yellow);
            Console.WriteLine(missingArgumentException.Message, ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("[ARGUMENT HELP]");
            CommandLineEngine.PrintHelp<T>(null);
            Console.WriteLine();
            WaitForEnter();
            return true;
         }

         return false;
      }

      #endregion

      #region Public Properties

      public static IConsole Console { get; } = new ConsoleProxy();

      /// <summary>Gets a value indicating whether this application was called with arguments.</summary>
      public bool HasArguments { get; private set; }

      #endregion

      #region Properties

      protected T Arguments { get; private set; }

      protected ICommandLineEngine CommandLineEngine { get; }

      private string[] Args { get; set; }

      #endregion

      #region Public Methods and Operators

      public virtual void RunWithCommand(ICommand command)
      {
         command.Execute();
      }

      #endregion

      #region Methods

      /// <summary>Executes the command that was specified in the command line arguments. 
      /// If no argument was specified but the IsDefaultCommand property was set at one of the commands, a simulated command</summary>
      /// <param name="useDefaultCommand">if set to <c>true</c> [use default command].</param>
      /// <returns></returns>
      protected virtual bool ExecuteCommand(bool useDefaultCommand)
      {
         var applicationArguments = new ArgumentClassInfo(typeof(T));
         if (!applicationArguments.HasCommands)
            return false;

         ICommand command = GetMappedCommand();
         if (command != null)
         {
            RunWithCommand(command);
            return true;
         }

         if (useDefaultCommand)
         {
            var defaultCommand = GetDefaultCommand();
            if (defaultCommand != null)
            {
               RunWithCommand(defaultCommand);
               return true;
            }
         }

         return false;
      }

      protected ICommand GetMappedCommand()
      {
         if (Arguments == null)
            return null;

         foreach (var propertyInfo in typeof(T).GetProperties())
         {
            if (propertyInfo.PropertyType.GetInterface(typeof(ICommand).FullName) != null)
            {
               var value = propertyInfo.GetValue(Arguments) as ICommand;
               if (value != null)
                  return value;
            }
         }

         return null;
      }

      protected ICommand GetDefaultCommand()
      {
         CommandInfo defaultComand = GetDefaultCommand(HasArguments);
         if (defaultComand == null)
            return null;
         
         var originalArgs = Args.ToList();
         originalArgs.Insert(0, defaultComand.Attribute.Name);

         CommandLineEngine.Map(originalArgs.ToArray(), Arguments);
         return defaultComand.PropertyInfo.GetValue(Arguments) as ICommand;
      }

      private static CommandInfo GetDefaultCommand(bool withParameters)
      {
         var argumentClassInfo = new ArgumentClassInfo(typeof(T));
         if (withParameters)
            return argumentClassInfo.CommandInfos.FirstOrDefault(c => c.Attribute.IsDefaultCommand && c.ArgumentType != null);

         return argumentClassInfo.CommandInfos.FirstOrDefault(c => c.Attribute.IsDefaultCommand && c.ArgumentType == null);
      }

      /// <summary>Called when after the arguments were initialized. This is the first method the arguments can be accessed</summary>
      protected virtual void OnArgumentsInitialized()
      {
      }

      protected virtual void RunWithoutArguments()
      {
         RunWith(Arguments);
      }

      protected void WaitForEnter(string waitText = "Press ENTER to continue.")
      {
         Console.WriteLine(waitText);
         Console.ReadLine();
      }

      #endregion
   }
}