//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________
using TP.ConcurrentProgramming.Presentation.Model;

namespace TP.ConcurrentProgramming.PresentationModelTest
{
    [TestClass]
    public class ModelAbstractAPITest
    {
        [TestMethod]
        public void SingletonConstructorTestMethod()
        {
            ModelAbstractApi instance1 = ModelAbstractApi.CreateModel(new Data.Dimensions(1,2,3));
            ModelAbstractApi instance2 = ModelAbstractApi.CreateModel(new Data.Dimensions(1, 2, 3));
            Assert.AreNotSame<ModelAbstractApi>(instance1, instance2);
        }
    }
}