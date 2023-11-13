using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetBackend.Models
{
    public class Message
    {
    }

    public interface IMessage
    {
        public Task ExecuteAsync();
    }

    public interface IQuery : IMessage
    {

    }

    public interface ICommand : IMessage
    {

    }

    public class OperationResult<TData>
    {
        public bool IsSuccess { get; set;  }
        public List<string> Errors { get; set; } = new List<string>();
        public TData Data { get; set;  }
    }
}
