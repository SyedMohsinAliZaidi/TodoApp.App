﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<TodoItemDatabase> Instance =
            new AsyncLazy<TodoItemDatabase>(async () =>
            {
                var instance = new TodoItemDatabase();
                try
                {
                    CreateTableResult result = await Database.CreateTableAsync<TodoItem>();
                }
                catch (Exception ex)
                {

                    throw;
                }
                return instance;
            });


        public TodoItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }

        public Task<List<TodoItem>> GetItemsAsync()
        {
            return Database.Table<TodoItem>().ToListAsync();
        }

        public Task<List<TodoItem>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<TodoItem> GetItemAsync(int id)
        {
            return Database.Table<TodoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TodoItem item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
