namespace _01_framework.Application
{
    public class OperationResult
    {
        public bool Sucssesed { get; set; }
        public string Message { get; set; }

        public OperationResult()
        {
            Sucssesed = false;
        }

        public OperationResult IsSucssed(string massage="عملیات با موفقیت انجام شد ")
        {
            Sucssesed=true;
            Message = massage;
            return this; 
        }

        public OperationResult Failed(string message)
        {
            Sucssesed=false;
            message = message;
            return this;
        }
    }
}
