using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 葡萄牙语配置
/// </summary>
public static class PortugueseLanguageConfig
{
    public static Dictionary<string, string> currentTexts = new Dictionary<string, string>()
    {
        {"Loading", "Carregando"},
        {"Level", "Nível"},
        {"LEVEL", "NÍVEL"},
        {"NoThanks", "Não, obrigado"},
        {"CLAIM", "RESGATAR"},
        {"Claim", "Resgatar"},
        {"ClaimAll", "Resgatar Tudo"},
        {"CONTINUE", "CONTINUAR"},
        {"Continue", "Continuar"},
        {"RESET", "REPETIR"},
        {"OK", "OK"},
        {"PrivacyPolicy", "Política de Privacidade"},
        {"TermsofService", "Termos de Serviço"},
        {"SETTINGS", "CONFIGURAÇÕES"},
        //sceneitem
        {"LockLvTips", "Desbloqueia no nível {0}"},
        {"ItemLimit", "Máximo de {0} usos de item por nível!"},
        //网络
        {"RETRY", "TENTAR NOVAMENTE"},
        {"NetworkStr", "Conexão de rede perdida. Verifique sua internet e tente novamente."},
        //评分
        {"EvaluationGamePanel_title", "Deixe sua avaliação:"},
        {"EvaluationGamePanel_EX", "Olá jogador, está gostando do nosso jogo? Agradeceríamos muito se você nos desse 5 estrelas na loja. Agradecemos seu apoio!"},
        {"EvaluationGamePanel_btn", "Avalie‑nos"},
        //tipsPanel
        {"ITEM", "ITEM"},
        {"NoItemHintTips", "Sem cartas móveis disponíveis!"},
        {"InsufficientDiamond", "Diamantes insuficientes!"},
        {"AdsNotReady", "O vídeo não está pronto, tente novamente mais tarde."},
        {"Limit", "Limite"},
        {"Free", "Grátis"},
        //addgameScene
        {"AddSceneItemPanel_ex1", "Toque automaticamente em 3 cobras."},
        {"AddSceneItemPanel_ex2", "Mostra dicas de todas as cobras que podem sair do tabuleiro em 10 segundos."},
        //Daily Mission
        {"DAILYMISSION", "MISSÃO DIÁRIA"},
        {"GO", "VÁ!"},
        {"SubtitleEx", "Assista vídeos e receba {0} instantaneamente"},
        {"DailyMissionEx2", "Assista {0} vídeos e receba recompensas {1} ({2})"},
        {"DailyMissionEx3", "Próximo horário de atualização {0}"},
        //gamelose
        {"gameloseEx", "Sem vidas restantes.\nNível falhou."},
        {"gamelosetitle", "DERROTA"},
        {"Revive", "Reviver"},
        {"Restart", "Reiniciar"},
        //otherreward
        {"Extras", "Extras"},
        {"otherrewardEx", "Assista vídeos para aumentar suas recompensas"},
        {"otherrewardEx2", "Assista mais {0} vídeos hoje para {1} {2}"},
        //引导
        {"Guide1Panel_ex", "Toque para Mover"},
        {"Collect", "Coletar"},
        {"Guide2Panel_title", "NOVA RECOMPENSA"},
        {"Guide3Panel_ex", "Todos os valores recebidos aparecem aqui, o saldo pode ser sacado."},
        {"Guide5Panel_ex", "Bloqueador Numérico\nO número indica quantas cobras devem sair antes que este bloqueador abra."},
        {"Guide6Panel_ex", "Buraco Negro\nEntrar no buraco negro também conta como fuga bem‑sucedida."},
        //tx
        {"TxPanel_myB", "Meu {0}"},
        {"TxPanel_levelText", "Passar De Nível"},
        {"TxPanel_ex1", "<color=#431422>{0}</color> O valor mínimo de {1} é <color=#431422>{2}</color>."},
        {"TxPanel_ex2", "Você precisa de mais <color=#431422>{0}</color>."},
        {"TxPanel_ex3", "Quanto maior o nível, maior o valor de saque!"},
        
        {"TxTipsPanel_FAQ", "FAQ"},
        {"TxFailedPanel_ex", "Você desbloqueará {0} após completar o Estágio {1}. Falta {2} estágios."},
        {"TxFailedPanel_ex2", "{0} falhou. Tente novamente mais tarde."},
        {"TxSucceedPanel_title", "{0} BEM‑SUCEDIDO"},
        {"TxSucceedPanel_ex", "Sua solicitação de {0} foi enviada e está em análise."},
        
        {"TxAccountPanel_title", "CONTA {0}"},
        {"TxAccountPanel_account", "Número Da Conta"},
        {"TxAccountPanel_accountEX", "Digite Sua Conta"},
        {"TxAccountPanel_email", "E‑mail"},
        {"TxAccountPanel_emailEX", "abcde@gmail.com"},
        {"TxAccountPanel_name", "Nome"},
        {"TxAccountPanel_nameEX", "Digite Seu Nome"},
        {"TxAccountPanel_phone", "CPF/CNPJ"},
        {"TxAccountPanel_phoneEX", "99999999999 ou 99999999999999"},
        {"TxAccountPanel_nameError", "O nome deve ser em inglês, sem caracteres especiais, com nome e sobrenome (exemplo: John Doe)."},
        {"TxAccountPanel_phoneError", "Número de telefone inválido."},
        {"TxAccountPanel_emailError", "E‑mail inválido. Insira um endereço válido (exemplo: user@domain.com)."},
        {"TxAccountPanel_accountError", "Erro de conta."},
        
        //records
        {"TxHistoryPanel_Title", "REGISTROS"},
        {"WDLStatus_REVIEWING", "Solicitado"},
        {"WDLStatus_PAYING", "Pagando"},
        {"WDLStatus_SUCCESS", "Bem‑sucedido"},
        {"WDLStatus_REJECTED", "Rejeitado"},
        {"WDLStatus_FAILED", "Falhou"},
        {"Month_1", "Jan"},
        {"Month_2", "Fev"},
        {"Month_3", "Mar"},
        {"Month_4", "Abr"},
        {"Month_5", "Mai"},
        {"Month_6", "Jun"},
        {"Month_7", "Jul"},
        {"Month_8", "Ago"},
        {"Month_9", "Set"},
        {"Month_10", "Out"},
        {"Month_11", "Nov"},
        {"Month_12", "Dez"},
        
        {"500", "Erro de sistema. Tente novamente mais tarde."},
        {"1001", "Dados da solicitação inválidos. Verifique seus dados."},
        {"1002", "Erro de configuração do app. Tente novamente mais tarde."},
        {"2001", "Jogador não encontrado. Tente novamente mais tarde."},
        {"3001", "Saldo insuficiente."},
        {"3002", "Nome do beneficiário inválido."},
        {"3003", "Número de telefone inválido."},
        {"3004", "Endereço de e‑mail inválido."},
        {"3005", "Já existe uma solicitação em andamento."},
      
        {"Special_Diamond_unit", "UiQ="},
        {"cht", "U2FjYXI="},
        {"Ch", "RGluaGVpcm8="},
        {"CH", "RElOSUVJUk8="},
        {"WD", "UkVUSVJBUg=="},
        {"Wd", "UmV0aXJhcg=="},
        {"wd", "cmV0aXJhcg=="},
        {"Wh", "U2FxdWU="},
        {"wh", "c2FxdWU="},
        {"Bl", "U2FsZG8="},

    };
}