using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain_Layer;

namespace TodoApp.Storage
{
    public class SqlTodoRepsoitory : ITodoRepository
    {
        private readonly SqlConnectionFactory connectionFactory;
        //all querys with join are left join, if not in a case where tags are empty it will not show the todo
        public SqlTodoRepsoitory(SqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }
        public async Task AddAsync(Todo todo)
        {
            string insertQuery = "INSERT INTO [dbo].[Todos]([TodoId], [Title], [Completed]) VALUES (@Id, @Title, @Completed)";
            string insertTags = "INSERT INTO [dbo].[Tags]([TagId], [Tag], [TodoId]) VALUES (@Id, @Tag, @TodoId)";
            using var connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(insertQuery, new { 
            Id=todo.Id,
            Title=todo.Title,
            Completed=todo.Completed
            });
            foreach(Tag tag in todo.ReadTags)
            {
                await connection.ExecuteAsync(insertTags, new
                {
                    Id =tag.Id,
                    Tag = tag.Title,
                    TodoId = todo.Id
                }) ;
            }
        }

        public async Task AddTagAsync(Tag tag)
        {
            string insertTag = "INSERT INTO Tags([TagId], [Tag], [TodoId]) VALUES (@Id, @Tag, @TodoId)";
            using var connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(insertTag, new
            {
                Id = tag.Id,
                Tag = tag.Title,
                TodoId=tag.TodoId
            }
                );
        }

        public async Task<Todo> FindByIdAsync(Guid id)
        { 
            var selectQuery = "SELECT * FROM Todos LEFT JOIN Tags on Todos.TodoId = Tags.TodoId WHERE Todos.TodoId=@id";
            var todoDictionary = new Dictionary<Guid, Todo>();
            using var connection = connectionFactory.CreateConnection();

            return (await connection.QueryAsync<Todo, Tag, Todo>(selectQuery,
                 (todo, tag) =>
                 {
                     Todo todoEntry;

                     if (!todoDictionary.TryGetValue(todo.Id, out todoEntry))
                     {
                         
                         todoEntry = todo;
                         todoDictionary.Add(todoEntry.Id, todoEntry);
                     }
                     todoEntry.AddTag(tag);
                     return todoEntry;
                 }
              , new { id } , splitOn: "TagId")).Distinct().SingleOrDefault(todo=>todo.Id==id);

         
        }
        public async Task<IReadOnlyList<Todo>> ListAsync(int limit)
        {
            if (limit < 0)
                throw new ArgumentOutOfRangeException();

            var selectQuery = "SELECT  * FROM Todos LEFT JOIN Tags on Todos.TodoId = Tags.TodoId  ORDER BY Todos.TodoId ";


            var todoDictionary= new Dictionary<Guid, Todo>();

            using var connection = connectionFactory.CreateConnection();
            return (await connection.QueryAsync<Todo, Tag, Todo>(selectQuery,
                (todo, tag) =>
                {
                    Todo todoEntry;

                    if (!todoDictionary.TryGetValue(todo.Id, out todoEntry))
                    {
                        todoEntry = todo;
                        todoDictionary.Add(todoEntry.Id, todoEntry);
                    }
                    todoEntry.AddTag(tag);
                    return todoEntry;
                }
                , new { limit }, splitOn: "TagId")).Distinct().Take(limit).ToList();
            

          
        }
        public async Task<bool> RemoveAsync(Guid id)
        {
            string deleteQuery = "DELETE FROM [dbo].[Todos] WHERE [Todos].TodoId = @Id";
       
            using var connection = connectionFactory.CreateConnection();
            var changes = await connection.ExecuteAsync(deleteQuery,new { id });
            return (changes > 0);
    }

        public async Task<bool> RemoveTagAsync(Guid id, string tag)
        {
            string deleteQuery = "DELETE FROM Tags WHERE TodoId=@id AND Tag=@tag";
            using var connection = connectionFactory.CreateConnection();
            var changes = await connection.ExecuteAsync(deleteQuery, new { id, tag });
            return (changes > 0);
        }

        public async Task<IReadOnlyList<Todo>> SearchAsync(string query)
        {
            string selectQuery = "SELECT * FROM [dbo].[Todos] LEFT JOIN [dbo].[Tags] ON Todos.TodoId=Tags.TodoId WHERE Title LIKE '%' + @query + '%'";
            var todoDictionary = new Dictionary<Guid, Todo>();
            using var connection = connectionFactory.CreateConnection();
            
            return (await connection.QueryAsync<Todo, Tag, Todo>(selectQuery,
                (todo, tag) =>
                {
                    Todo todoEntry;

                    if (!todoDictionary.TryGetValue(todo.Id, out todoEntry))
                    {
                        todoEntry = todo;
                        todoDictionary.Add(todoEntry.Id, todoEntry);
                    }
                    todoEntry.AddTag(tag);
                    return todoEntry;
                }
                , new { query }, splitOn: "TagId")).Distinct().ToList();
            
            
        }

        public async Task<IReadOnlyList<Todo>> SearchByTagAsync(string tagQuery)
        {
            string selectQuery = "SELECT * FROM (SELECT [Todos].[TodoId],[Title],[Completed] FROM Todos LEFT JOIN Tags ON Todos.TodoId=Tags.TodoId WHERE Tag LIKE '%' + @tagQuery+ '%') AS t LEFT JOIN Tags On t.TodoId=Tags.TodoId ";
            var todoDictionary = new Dictionary<Guid, Todo>();
            using var connection = connectionFactory.CreateConnection();

           
             return (await connection.QueryAsync<Todo, Tag, Todo>(selectQuery,
      (todo, tag) =>
      {
          Todo todoEntry;
          if (!todoDictionary.TryGetValue(todo.Id, out todoEntry))
          {
              todoEntry = todo;
              todoDictionary.Add(todoEntry.Id, todoEntry);
          }
          todoEntry.AddTag(tag);
          return todoEntry;
      }
      , new { tagQuery }, splitOn: "TagId")).Distinct().ToList();
         
        }

        public async Task UpdateAsync(Todo todo)
        { 
            string updateQuery = "UPDATE Todos SET Title = @Title, Completed= @Completed WHERE TodoId = @Id";
            string updateTagsQuery = "UPDATE Tags SET Tag = @Tag WHERE TodoId = @Id"; 
            using var connection = connectionFactory.CreateConnection();
            await connection.ExecuteAsync(updateQuery, new
            {
                Title = todo.Title,
                Completed = todo.Completed,
                Id = todo.Id
            }) ;
            foreach (var tag in todo.ReadTags) {
                if (tag is null) break;
                await connection.ExecuteAsync(updateTagsQuery, new
                {
                    Tag = tag.Title,
                    Id = todo.Id
                });;
                    }
        }
    }
}
