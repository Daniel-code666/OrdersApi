using Orders.Application.Common.Enum;

namespace Orders.Application.Common
{
    public sealed class BusinessException : Exception
    {
        public ErrorType ErrorType { get; }

        public BusinessException(ErrorType error_type) : base(error_type.ToString())
        {
            ErrorType = error_type;
        }

        public BusinessException(ErrorType error_type, string message) : base(message)
        {
            ErrorType = error_type;
        }
    }
}
