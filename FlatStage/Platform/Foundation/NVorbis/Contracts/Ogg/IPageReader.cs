﻿using System;

namespace FlatStage.Foundation.NVorbis.Contracts.Ogg;

interface IPageReader : IDisposable
{
    void Lock();
    bool Release();

    long ContainerBits { get; }
    long WasteBits { get; }

    bool ReadNextPage();

    bool ReadPageAt(long offset);
}