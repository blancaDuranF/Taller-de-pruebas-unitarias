using AutoMapper;
using Moq;
using Ordering.Application.Contracts;
using Ordering.Application.Features.Queries;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ordering.Test.Features.Queries
{
    public class GetOrdersListQueryHandlerTest
    {
        [Fact]
       
        public async Task Handler_Should_GetAsync_And_Return_OrderList()
        {
            //Arrange
            Mock<IGenericRepository<Order>> _repositoryMock = new();
            Mock<IMapper> _mapperMock = new();

            var getOrdersListQueryHandler = new GetOrdersListQueryHandler(_repositoryMock.Object, _mapperMock.Object);
            var getOrderListQuery = new GetOrdersListQuery()
            {
                UserName = "Blanca",
            };

            _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                .ReturnsAsync(new List<Order>());

            _mapperMock.Setup(mapper => mapper.Map<List<OrdersViewModel>>(It.IsAny<List<Order>>()))
                .Returns(new List<OrdersViewModel>());


            //Act
            var result = await getOrdersListQueryHandler.Handle(getOrderListQuery, CancellationToken.None);


            //Asserts
            _repositoryMock.Verify(repo => repo.GetAsync(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
            Assert.IsType<List<OrdersViewModel>>(result);

            _mapperMock.Verify(map => map.Map<List<OrdersViewModel>>(It.IsAny<List<Order>>()), Times.Once);
        }
    }
}
