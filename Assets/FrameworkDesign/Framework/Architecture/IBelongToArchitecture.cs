using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：解决 Architecture 中的递归调用
 * 
 * 标记这个 Architecture 是属于哪个架构的
 * 
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface IBelongToArchitecture
    {
        IArchitecture GetArchiteccture();
    }
}
