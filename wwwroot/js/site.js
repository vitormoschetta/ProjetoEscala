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



function AtualizarLocal(local) {  

    var pessoa = document.getElementById("Id").value;
        
    if (document.getElementById(local).checked == true){ 

        var url = $("#adicionar-local").data("url");  
        
        $.post(url, {localId: local, pessoaId: pessoa}, function (data)
        {                   
            $("#listaLocal").empty();
            $("#listaLocal").html(data);
        }); 
        alert("Local Adicionado!");
    }
    else {
        var url = $("#retirar-local").data("url");         

        $.post(url, {localId: local, pessoaId: pessoa}, function (data)
        {       
            
            $("#listaLocal").empty();
            $("#listaLocal").html(data);
        }); 
        alert("Local Removido!");
    }    

}



function LimparTudoAJAX() {

    var escala = document.getElementById("escala-id").value;
    //var url = "/ItemQuadro/LimparTudo";
    var url = $("#LimparTudoAJAX").data("url");  

    //O correto era limpar detro da função abaixo. Porém como o SqLite tem baixa
    //performance, estou limpando antes para ficar melhor a usabilidade, 
    //enquanto o banco trabalha em segundo plano.
    //$("#tabelaQuadro").empty();
    
    $.post(url, {escalaId: escala}, function (data)
    {
        $("#tabelaQuadro").empty();
    }); 
}