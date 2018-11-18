using System.Collections.Generic;

namespace Splitwise.Model.Validators
{
    public interface IValidator<T>
    {
        bool Validate(T model, out IList<string> errorMessages);
    }
}
