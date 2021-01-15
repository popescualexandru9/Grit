using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Grit.Services
{
    public class OpenExerciseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public OpenExerciseException() { }

        public OpenExerciseException(HttpStatusCode statusCode)
            => StatusCode = statusCode;

        public OpenExerciseException(HttpStatusCode statusCode, string message) : base(message)
            => StatusCode = statusCode;

        public OpenExerciseException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
            => StatusCode = statusCode;
    }
    
}