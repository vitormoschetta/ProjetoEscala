using System;    
using System.Collections.Generic;
using MySql.Data.MySqlClient;   
using ProjetoEscala.Models;

namespace ProjetoEscala.Context
{
    public class Conexao
    {        
        public string ConnectionString { get; } 

        public Conexao()   
        {    
            this.ConnectionString = "Server=127.0.0.1;DataBase=ProjetoEscala;User id=root;Password=123";   
        }    
    
        private MySqlConnection GetConnection()    
        {    
            return new MySqlConnection(ConnectionString);    
        }  
        
        public IList<int> ListarEscalaId()  
        {
            IList<int> lista = new List<int>();   
            using (MySqlConnection conn = GetConnection()){
                conn.Open();      
                string comando = "SELECT id FROM escala order by id desc limit 12";                          
                MySqlCommand cmd = new MySqlCommand(comando, conn);  
        
                using (var reader = cmd.ExecuteReader()){  
                    while (reader.Read()){ 
                        lista.Add(Convert.ToInt32(reader["id"]));                   
                    } 
                }  
                 conn.Close();                    
            }                                                
            return lista; 
        }

        public IList<ItemQuadro> ListaItemUltimoQuadro (int escalaId)  
        {
            int escalaIdAnterior = new int();
            IList<int> listaEscalaId = ListarEscalaId();
            foreach(var item in listaEscalaId){
                if (item < escalaId){
                    escalaIdAnterior = item;
                    break;
                }
            }

            IList<int> lista = new List<int>();   
            IList<ItemQuadro> listaUltimosItemQuadro = new List<ItemQuadro>();

            if (escalaIdAnterior == 0){
                listaUltimosItemQuadro = null;
                return listaUltimosItemQuadro;
            }

            
            using (MySqlConnection conn = GetConnection()){
                conn.Open();      
                string comando = "SELECT pessoalocal.LocalId FROM pessoalocal";           
                comando = comando + " INNER JOIN itemquadro ON itemquadro.localId = pessoalocal.localId";
                comando = comando + " INNER JOIN quadro ON quadro.id = itemquadro.quadroId";
                comando = comando + " INNER JOIN escala ON escala.id = quadro.escalaId";
                comando = comando + " WHERE escala.id = " + escalaIdAnterior;
                comando = comando + " GROUP BY pessoalocal.localId";            
                MySqlCommand cmd = new MySqlCommand(comando, conn);  
        
                using (var reader = cmd.ExecuteReader()){  
                    while (reader.Read()){ 
                        lista.Add(Convert.ToInt32(reader["localId"]));                   
                    } 
                }  

                int quantidadeLocal = lista.Count;                
                for (int i = 0; i < quantidadeLocal; i++){
                    comando = "SELECT itemquadro.PessoaId, itemquadro.LocalId FROM itemquadro";
                    comando = comando + " INNER JOIN quadro ON quadro.id = itemquadro.quadroId";
                    comando = comando + " INNER JOIN escala ON escala.id = quadro.escalaId";
                    comando = comando + " WHERE escala.id = " + escalaIdAnterior;
                    comando = comando + " AND itemquadro.LocalId = " + lista[i];
                    comando = comando + " ORDER BY itemquadro.id desc"; 
                    comando = comando + " LIMIT 1"; //SQL SERVER TEM QUE MODIFICAR ESSA CLAUSULA
                    cmd = new MySqlCommand(comando, conn);

                    using (var reader = cmd.ExecuteReader()){  
                        while (reader.Read()){ 
                                listaUltimosItemQuadro.Add(new ItemQuadro(){
                                PessoaId = Convert.ToInt32(reader["PessoaId"]),
                                LocalId = Convert.ToInt32(reader["LocalId"])});                                
                        }                                                
                    } 
                }
            conn.Close();                    
            }                                                
            return listaUltimosItemQuadro;              
        }


        public IList<int> ListarLocalIdAgrupado(int escalaId)  
        {      
            IList<int> lista = new List<int>();   
            using (MySqlConnection conn = GetConnection()){
                conn.Open();      
                string comando = "SELECT pessoalocal.localId FROM pessoalocal";
                comando = comando + " INNER JOIN itemquadro ON itemquadro.localId = pessoalocal.localId";
                comando = comando + " INNER JOIN quadro ON quadro.id = itemquadro.quadroId";
                comando = comando + " INNER JOIN escala ON escala.id = quadro.escalaId";
                comando = comando + " WHERE escala.id = " + escalaId;
                comando = comando + " GROUP BY pessoalocal.localId";            
                MySqlCommand cmd = new MySqlCommand(comando, conn);  
        
                using (var reader = cmd.ExecuteReader()){  
                    while (reader.Read()){ 
                        lista.Add(Convert.ToInt32(reader["localId"]));                   
                    } 
                }  
                conn.Close();
            }  
            return lista;  
        }


        public IList<int> ListarPessoaIdDeLocalEspecifico(int localId)  
        {      
            IList<int> lista = new List<int>();   
            using (MySqlConnection conn = GetConnection()){
                conn.Open();      
                string comando = "SELECT pessoaId FROM pessoalocal";
                comando = comando + " INNER JOIN pessoa ON pessoa.id = pessoalocal.pessoaId";
                comando = comando + " where localId = " + localId;        
                comando = comando + " and pessoa.ativo = 2";
                MySqlCommand cmd = new MySqlCommand(comando, conn);  
        
                using (var reader = cmd.ExecuteReader()){  
                    while (reader.Read()){ 
                        lista.Add(Convert.ToInt32(reader["pessoaId"]));                   
                    } 
                }  
                conn.Close();
            }  
            return lista;  
        }


        public IList<int> ListarQuadroIdPorEscala(int escalaId)  
        {      
            IList<int> lista = new List<int>();   
            using (MySqlConnection conn = GetConnection()){
                conn.Open();      
                string comando = "SELECT id FROM quadro";
                comando = comando + " where escalaId = " + escalaId;        
                MySqlCommand cmd = new MySqlCommand(comando, conn);  
        
                using (var reader = cmd.ExecuteReader()){  
                    while (reader.Read()){ 
                        lista.Add(Convert.ToInt32(reader["id"]));                   
                    } 
                }  
                conn.Close();
            }  
            return lista;  
        }

        
    }
}