namespace ConsoLovers.UnitTests.DIContainer.Testclasses
{
   using ConsoLovers.ConsoleToolkit.DIContainer;

   internal class NoDependanceAttribute
   {
      public IFoo Foo { get; set; }

      public IBar Bar { get; set; }
   }

   internal class OneDependanceAttribute
   {
      [Dependency]
      public IFoo Foo { get; set; }

      public IBar Bar { get; set; }
   }

   internal class PrivateDependanceAttribute
   {
      [Dependency]
      public IFoo Foo { get; private set; }

      public IBar Bar { get; set; }
   }

   internal interface IBar
   {
   }

   internal interface IFoo
   {
   }
}