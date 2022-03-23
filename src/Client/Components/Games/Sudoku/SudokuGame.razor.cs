using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Client;
using Client.Common;
using MudBlazor;
using Fluxor;
using Client.Constants;
using Blazor.UI.Library;
using Blazor.UI.Library.Models;
using Blazor.UI.Library.Components;

namespace Client.Components.Games.Sudoku;

public partial class SudokuGame
{
    private int? Pointer;
    private List<int> blocks;

    public SudokuGame()
    {
        blocks = new();
        foreach (var item in Enumerable.Range(1, 81))
        {
            blocks.Add(item);
        }
    }

    private string GetBlockCssClass(int n)
     => Pointer == n ? "block-selected" : string.Empty;

    private void SetPointer(int n)
    {
        if (Pointer == n)
        {
            Pointer = null;
        }
        else
        {
            Pointer = n;
        }      
        StateHasChanged();
    }
}