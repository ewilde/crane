﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crane.Core
{
    public interface IGuidGenerator
    {
        Guid Create();
    }
}
