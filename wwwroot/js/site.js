function SelectEscala(params){
    //var url = "/Quadro/QuadroPorEscala";
    var url = $("#filtro-escala").data("url");  
    
    $.post(url, {escalaId: params}, function (data)
    {
        $("#tabelaQuadro").empty();
        $("#tabelaQuadro").html(data);
    }); 
}


function CheckedDestaque() {
    var check = document.getElementsByName("Destaque"); 

    for (var i=0;i<check.length;i++){ 
        if (check[i].checked == true){ 
            document.getElementsByName("Destaque").value = 'S';
        }
        else {
            document.getElementsByName("Destaque").value = 'N';
        }
    }
}