using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Questao5.Domain.Exceptions;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Services.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = (int)HttpStatusCode.InternalServerError; // Status code padrão para outras exceções
            TipoValidacaoEnum tipo = TipoValidacaoEnum.INTERNAL_ERROR;

            if (exception is InvalidAccountException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                tipo = TipoValidacaoEnum.INVALID_ACCOUNT;
            }
            if (exception is InactiveAccountException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                tipo = TipoValidacaoEnum.INACTIVE_ACCOUNT;
            }
            if (exception is InvalidValueException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                tipo = TipoValidacaoEnum.INVALID_VALUE;
            }
            if (exception is InvalidTypeException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                tipo = TipoValidacaoEnum.INVALID_TYPE;
            }


            var result = new ObjectResult(new
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Tipo = tipo
            })
            {
                StatusCode = statusCode,
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
