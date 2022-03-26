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
    private List<Sudoku.Block> blocks;
    private Sudoku.Solver _solver;
    public SudokuGame()
    {
        var test = ",,,,,6,1,8,,,8,,,7,,3,9,,,3,,4,,,7,,,,,,5,,,8,,,9,,,,,,,,1,,,7,,,1,,,,,,9,,,3,,5,,,7,5,,6,,,3,,,2,6,9,,,,,";
        var list = test.Split(',');
        blocks = list
            .Select((e,i) => new Sudoku.Block(i, string.IsNullOrEmpty(e) ? null : int.Parse(e), !string.IsNullOrEmpty(e)))
            .ToList();
        _solver = new(blocks);
    }

    private string GetBlockCssClass(Sudoku.Block b)
     => Pointer == b.Id ? "block-selected" : string.Empty;

    private void SetPointer(Sudoku.Block b)
    {
        if (Pointer == b.Id)
        {
            Pointer = null;
        }
        else
        {
            Pointer = b.Id;
        }      
        StateHasChanged();
    }
}