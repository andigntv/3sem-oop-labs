﻿namespace Backups.Exceptions;

public class FileException : Exception
{
    public FileException(string message)
        : base(message) { }
}