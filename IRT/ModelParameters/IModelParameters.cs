using System.Security.Cryptography.X509Certificates;

namespace IRT.ModelParameters
{
    public interface IModelParameters
    {
        IModelParameters DeepCopy();
    }
}