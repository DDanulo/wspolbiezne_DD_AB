//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.BusinessLogic;

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
  [TestClass]
  public class BusinessLogicAbstractAPIUnitTest
  {
    [TestMethod]
    public void BusinessLogicConstructorTestMethod()
    {
      BusinessLogicAbstractAPI instance1 = BusinessLogicAbstractAPI.CreateBusinessLogicLayer(new Data.Dimensions(10, 10, 10));
      BusinessLogicAbstractAPI instance2 = BusinessLogicAbstractAPI.CreateBusinessLogicLayer(new Data.Dimensions(10, 10, 10));
      Assert.AreNotSame(instance1, instance2);
    }
  }
}