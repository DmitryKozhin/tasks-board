using System;

using FluentAssertions;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Features.Columns;
using TasksBoard.Backend.Infrastructure.Errors;
using TasksBoard.Tests.Features.Tasks;

using Xunit;
using Xunit.Abstractions;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Columns
{
    public class DetailsTest : BackendTestBase
    {
        public DetailsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Details_CheckExistingColumn_ReturnTask()
        {
            var userId = await CreateUser();
            var boardId = await ColumnTestHelper.CreateBoard(ContextInjector.WriteContext, userId);
            var columnHeader = "column";
            var columnColor = "color";
            var columnId = await ColumnTestHelper.CreateColumn(ContextInjector.WriteContext, userId, boardId, columnHeader, columnColor);

            var query = new Details.Query(columnId);
            var columnEnvelope = await SendAsync(query);

            columnEnvelope.Should().NotBeNull();
            columnEnvelope.Column.Should().NotBeNull();
            columnEnvelope.Column.Header.Should().BeEquivalentTo(columnHeader);
            columnEnvelope.Column.Color.Should().BeEquivalentTo(columnColor);
        }

        [Fact]
        public async Task Details_CheckNotExistingColumn_ReturnException()
        {
            var userId = await CreateUser();
            var boardId = await ColumnTestHelper.CreateBoard(ContextInjector.WriteContext, userId);
            var columnId = await ColumnTestHelper.CreateColumn(ContextInjector.WriteContext, userId, boardId);

            var rndNotExistingGuid = Guid.NewGuid();
            while (rndNotExistingGuid == columnId)
                rndNotExistingGuid = Guid.NewGuid();

            var query = new Details.Query(rndNotExistingGuid);
            Func<Task> getColumnDetailFunc = async () => await SendAsync(query);
            getColumnDetailFunc.Should().Throw<RestException>();
        }
    }
}