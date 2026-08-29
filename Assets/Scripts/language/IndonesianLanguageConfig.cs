using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 印尼语
/// </summary>
public static class IndonesianLanguageConfig
{
    public static Dictionary<string, string> currentTexts = new Dictionary<string, string>()
    {
        {"Loading", "Memuat"},
        {"Level", "Tingkat"},
        {"LEVEL", "TINGKAT"},
        {"NoThanks", "Tidak, Terima Kasih"},
        {"CLAIM", "KLAIM"},
        {"Claim", "Klaim"},
        {"ClaimAll", "Klaim Semua"},
        {"CONTINUE", "LANJUTKAN"},
        {"Continue", "Lanjutkan"},
        {"RESET", "ULANGI"},
        {"OK", "OK"},
        {"PrivacyPolicy", "Kebijakan Privasi"},
        {"TermsofService", "Syarat Layanan"},
        {"SETTINGS", "PENGATURAN"},
        //sceneitem
        {"LockLvTips", "Terbuka di tingkat {0}"},
        {"ItemLimit", "Maksimal {0} penggunaan item per tingkat!"},
        //网络
        {"RETRY", "COBA LAGI"},
        {"NetworkStr", "Koneksi internet terputus. Periksa jaringan Anda dan coba lagi."},
        //评分
        {"EvaluationGamePanel_title", "Berikan penilaian Anda:"},
        {"EvaluationGamePanel_EX", "Pemain terkasih, apakah Anda menyukai game ini? Kami sangat menghargai jika Anda memberi bintang 5 di toko aplikasi. Terima kasih atas dukungan Anda!"},
        {"EvaluationGamePanel_btn", "Beri Nilai"},
        //tipsPanel
        {"ITEM", "ITEM"},
        {"NoItemHintTips", "Tidak ada kartu yang bisa digerakkan!"},
        {"InsufficientDiamond", "Diamond tidak cukup!"},
        {"AdsNotReady", "Video belum siap, silakan coba lagi nanti."},
        {"Limit", "Batas"},
        {"Free", "Gratis"},
        //addgameScene
        {"AddSceneItemPanel_ex1", "Mengetuk ular secara otomatis sebanyak 3 kali."},
        {"AddSceneItemPanel_ex2", "Menampilkan petunjuk semua ular yang bisa keluar papan dalam 10 detik."},
        //Daily Mission
        {"DAILYMISSION", "MISI HARIAN"},
        {"GO", "AYO!"},
        {"SubtitleEx", "Tonton video dan dapatkan {0} secara instan"},
        {"DailyMissionEx2", "Tonton {0} video dan dapatkan hadiah {1} ({2})"},
        {"DailyMissionEx3", "Waktu pembaruan berikutnya {0}"},
        //gamelose
        {"gameloseEx", "Nyawa habis.\nTingkat gagal."},
        {"gamelosetitle", "KALAH"},
        {"Revive", "Hidup Kembali"},
        {"Restart", "Mulai Ulang"},
        //otherreward
        {"Extras", "Hadiah Tambahan"},
        {"otherrewardEx", "Tonton video untuk menambah hadiah Anda"},
        {"otherrewardEx2", "Tonton {0} video lagi hari ini untuk {1} {2}"},
        //引导
        {"Guide1Panel_ex", "Ketuk untuk Bergerak"},
        {"Collect", "Kumpulkan"},
        {"Guide2Panel_title", "HADIAH BARU"},
        {"Guide3Panel_ex", "Semua nilai yang diterima ditampilkan di sini, saldo dapat dicairkan."},
        {"Guide5Panel_ex", "Pemblokir Angka\nAngka menunjukkan berapa banyak ular harus keluar sebelum pemblokir terbuka."},
        {"Guide6Panel_ex", "Lubang Hitam\nMasuk ke lubang hitam juga dihitung sebagai pelarian berhasil."},
        //tx
        {"TxPanel_myB", "{0} Saya"},
        {"TxPanel_levelText", "Selesaikan Tingkat"},
        {"TxPanel_ex1", "<color=#431422>{0}</color> Jumlah minimum {1} adalah <color=#431422>{2}</color>."},
        {"TxPanel_ex2", "Anda masih butuh <color=#431422>{0}</color> lagi."},
        {"TxPanel_ex3", "Semakin tinggi tingkat, semakin besar jumlah penukaran!"},
        
        {"TxTipsPanel_FAQ", "FAQ"},
        {"TxFailedPanel_ex", "Anda bisa membuka {0} setelah selesaikan Tahap {1}. Masih butuh {2} tahap lagi."},
        {"TxFailedPanel_ex2", "{0} gagal. Silakan coba lagi nanti."},
        {"TxSucceedPanel_title", "{0} BERHASIL"},
        {"TxSucceedPanel_ex", "Permintaan {0} sedang diproses dan dalam peninjauan."},
        
        {"TxAccountPanel_title", "AKUN {0}"},
        {"TxAccountPanel_account", "Nomor Akun"},
        {"TxAccountPanel_accountEX", "Masukkan Akun Anda"},
        {"TxAccountPanel_email", "Email"},
        {"TxAccountPanel_emailEX", "abcde@gmail.com"},
        {"TxAccountPanel_name", "Nama"},
        {"TxAccountPanel_nameEX", "Masukkan Nama Anda"},
        {"TxAccountPanel_phone", "CPF/CNPJ"},
        {"TxAccountPanel_phoneEX", "99999999999 atau 99999999999999"},
        {"TxAccountPanel_nameError", "Nama harus pakai bahasa Inggris, tanpa karakter khusus, wajib nama depan dan belakang (contoh: John Doe)."},
        {"TxAccountPanel_phoneError", "Nomor telepon tidak valid."},
        {"TxAccountPanel_emailError", "Email tidak valid. Masukkan alamat yang benar (contoh: user@domain.com)."},
        {"TxAccountPanel_accountError", "Kesalahan akun."},
        
        //records
        {"TxHistoryPanel_Title", "CATATAN"},
        {"WDLStatus_REVIEWING", "Diminta"},
        {"WDLStatus_PAYING", "Sedang Dibayar"},
        {"WDLStatus_SUCCESS", "Berhasil"},
        {"WDLStatus_REJECTED", "Ditolak"},
        {"WDLStatus_FAILED", "Gagal"},
        {"Month_1", "Jan"},
        {"Month_2", "Feb"},
        {"Month_3", "Mar"},
        {"Month_4", "Apr"},
        {"Month_5", "Mei"},
        {"Month_6", "Jun"},
        {"Month_7", "Jul"},
        {"Month_8", "Agu"},
        {"Month_9", "Sep"},
        {"Month_10", "Okt"},
        {"Month_11", "Nov"},
        {"Month_12", "Des"},
        
        {"500", "Kesalahan sistem. Silakan coba lagi nanti."},
        {"1001", "Informasi permintaan tidak valid. Periksa input Anda."},
        {"1002", "Konfigurasi aplikasi error. Silakan coba lagi nanti."},
        {"2001", "Pemain tidak ditemukan. Silakan coba lagi nanti."},
        {"3001", "Saldo tidak cukup."},
        {"3002", "Nama penerima tidak valid."},
        {"3003", "Nomor telepon tidak valid."},
        {"3004", "Alamat email tidak valid."},
        {"3005", "Ada pesanan yang sedang berjalan."},

        {"Special_Diamond_unit", "UnA="},
        {"cht", "dGFyaWsgZGFuYQ=="},
        {"Ch", "VWFuZyBUdW5haQ=="},
        {"CH", "VUFORyBURU5BSQ=="},
        {"WD", "VEFSSUs="},
        {"Wd", "VGFyaWs="},
        {"wd", "dGFyaWs="},
        {"Wh", "UGVuYXJpa2Fu"},
        {"wh", "cGVuYXJpa2Fu"},
        {"Bl", "U2FsZG8="},
    };
}