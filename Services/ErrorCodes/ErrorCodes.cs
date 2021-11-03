using System;
using DevExpress.Xpo;

namespace Services.ErrorCodesNS
{

public enum ErrorCodes
    {
        None = 0,
        NotFound = 1,
        Null = 2,
        TitleDuplicated = 3,
        EmptyMandatoryFields = 4,
        IsEmpty = 5,
        ServerError = 6
    }
}