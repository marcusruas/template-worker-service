using System;
using System.Collections.Generic;
using Bogus;

namespace WorkerTemplate.SharedKernel.Tests.Models
{
    public abstract class StubGenerator<T> where T : class
    {
        public StubGenerator()
        {
            Faker = new Faker("en_us");
            FakerObject = new Faker<T>();
            Random = new Random();
        }

        protected Faker Faker;
        protected Faker<T> FakerObject;
        protected Random Random;

        public abstract void CreateObject();

        public T BuildFirst()
            => FakerObject.Generate();

        public IEnumerable<T> BuildToList(int quantidadeRegistros = 10)
            => FakerObject.Generate(quantidadeRegistros);
    }
}