using System;    
using System.Collections.Generic;
using MySql.Data.MySqlClient;   

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
        


        public IList<int> ListaLocalIdAgrupado(int escalaId)  
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


        public IList<int> ListaPessoaIdDeLocalEspecifico(int localId)  
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


        public IList<int> ListaQuadroIdPorEscala(int escalaId)  
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