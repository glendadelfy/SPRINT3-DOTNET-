//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//public static class DbSetMockExtensions
//{
//    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
//    {
//        var queryable = sourceList.AsQueryable();

//        var mockDbSet = new Mock<DbSet<T>>();

//        // Configurar o comportamento para IQueryable
//        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
//        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
//        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
//        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

//        // Configurar o comportamento para métodos assíncronos, como ToListAsync
//        mockDbSet
//            .Setup(m => m.ToListAsync(It.IsAny<CancellationToken>()))
//            .ReturnsAsync(sourceList);

//        return mockDbSet;
//    }
//}