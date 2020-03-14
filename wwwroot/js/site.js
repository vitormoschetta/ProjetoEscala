function SelectEscala(params){
    //var url = "/Quadro/QuadroPorEscala";
    var url = $("#filtro-escala").data("url");  
    
    $.post(url, {escalaId: params}, function (data)
    {
        $("#tabelaQuadro").empty();
        $("#tabelaQuadro").html(data);
    }); 
}