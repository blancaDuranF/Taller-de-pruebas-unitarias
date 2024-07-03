using Ordering.Application.Features.Commands.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ordering.Application.Contracts;
using Ordering.Domain.Entities;
using AutoMapper;

namespace Ordering.Test.Features.Commands
{
    public class CheckoutOrderCommandHandlerTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Handle_Should_Call_AddAsync_And_Return_Id(int id)
        {
            //Arrange
            Mock<IGenericRepository<Order>> _repositoryMock = new();
            Mock<IMapper> _mapperMock = new();

            var checkoutOrderCommandHandler = new CheckoutOrderCommandHandler(_repositoryMock.Object, _mapperMock.Object);

            var checkoutOrderCommandTest = new CheckoutOrderCommand()
            {
                Address = "correo@gmail.com",
                FirstName = "Blanca",
                LastName = "Duran",
                PaymentMethod = 1,
                TotalPrice = 10,
                UserName = "BlancaDuran"
            };

            _repositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(new Order { Id = id });

            _mapperMock.Setup(mapper => mapper.Map<Order>(checkoutOrderCommandTest))
                .Returns(new Order { Id = id });


            //Act
            var result = await checkoutOrderCommandHandler.Handle(checkoutOrderCommandTest, CancellationToken.None);

            //Assert
            Assert.Equal(id, result);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Order>()), Times.Once); //Verify permite saber si algo se manda a llamar el numero de vece que le indicas 
            _mapperMock.Verify(map=> map.Map<Order>(checkoutOrderCommandTest), Times.Once);

        }

        [Fact]
        public void Should_Test_Assert()
        {
            //Assert -> Sirve para comparar 
            Assert.Equal(4, 2 + 2); //compara valores
            Assert.NotEqual(5, 2 + 2); //verificar que no sean iguales 
            Assert.True(2+2==4); //Recibe condicion boleana  - debe ser verdadero
            Assert.False(2+2==4); //Recibe condicion boleana  - debe ser falso

            //NULL - NOTNULL
            object? obj = null;
            Assert.Null(obj); //verifica que sea nulo

            obj = new();
            Assert.NotNull(obj); //verifica que NO sea nulo

            //BUSQUEDA DENTRO DE LISTAS 
            var List = new List<int> { 1, 2, 3, 4 };
            Assert.Contains(2, List); // busca si un valor esta en una lista
            Assert.DoesNotContain(5, List); //La lista no debe de contener el valor
            Assert.NotEmpty(List);//Verifica que la lista no este vacía
            Assert.Empty(List);//Verifica que la lista este vacía
            

            //Tipo Type
            Assert.IsType<int>(1); 
            Assert.IsNotType<string>(1);

            //PARA MANEJO DE EXCEPCIONES
            Assert.Throws<InvalidOperationException>(() => MethodTrowsException());
            
            var obj1 = new object();
            var sameObj = obj1;
            Assert.Same(sameObj, obj1);
            Assert.NotSame(sameObj, obj1);
        }

        //codigo solo para ejemplo de Assert.Throws
        void MethodTrowsException() => throw new InvalidOperationException();


    }
}
