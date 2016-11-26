namespace Jasily.ComponentModel.Editor
{
    internal interface IEditor
    {
        void WriteToObject(object obj);

        void ReadFromObject(object obj);
    }
}