using System;
using System.Collections.Generic;

namespace ZTImage.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string safe = "jJvdv_VXdCCZwVwTZr48x7ENfp1GuwfArB2kJhtt_p9DqtFJ_Lu_gBDXBLnZtnCMWlGPtQfgzDkoA0hMzbFkpDC4yw3Gf6mE3gkA3b_ofQ_WX5X43v8EZ2Rm8S28u1nxkG6ViwcOLTkIU33GaNJbOPlkTi4nP-ZC3LMP_5RDD9Aj-vlKHSqeRxj8XEx63wvwx0Ajtn2K9Z2j4mGZIzHNonKgjvPamqfjjahpia8zFjWJffEef8d-ZdAERlcK7_J01YbZuSP2isXJo68bk1Y097GARi_dZAGsZgYbvqr1vVxuWLePYqoJ2F2zarvIP3hMlD_tSB87sYlvssJxpfcl_VZWo9poe_Gp6Ug-5Q3DX8XqAJjKJ-yJ8D3jP_aE4-kksSbt-9ZoQbtzkpb9w3lcO2xrcmtb2oyJ66DP3QgnH39dYpbc1eOuSU1xouUdV7QP0uB5k1yLz99SQcDv1yk-MfTa8Bhwr_KGibzz2fcT5ne46jAp3PpTjMlTU7dQn1wKs0pe7AkjNaXBjsnusjXZmKQ7r21c39moE437B7UYYHpLmxJ5kgpd9BX2h-tMd33lpRCLJm0REQyqFl-gwj4j0E-upqgRWJRif3v_WAy2OwllKrhyRup_gDcAbSA3NsiiwezT1XHPns9y7jp8zlBCTJINDsHoEpwkPPP8e4BtP3r8eWQr2ucYMerk3V_MOfYkIpKKAJowu1tSbK1twa2a6DPAWTimh_ROvqn6JoDJFvvrPVRDyPC8oAUHc0e1Ha7vsXKoq1y94O5xXbkkNxfV0JlN1e5pjeLN5H4BbFEqWwRWVqPaaHvxqelIPuUNw1_FL0ieMSvOmqMuR-siTCcoCzL38h4SuEpC_rKDBnPUt-P-46jXPGSBW0AtoJyetkP1ulT2Yd6FShVgkKO1ppAGpmcH2xtvExXEICWhIEucvOriqMetR9w5X9f9tqqV7s4npS47pE0ZxDIRbAoT-zUQ8__qD3YN89kEmcqFilLMlaPgaV-RRGVvR1sMm75hq1Vf_9UyTuPn78SCiBVkunsyVhJTBjNtTjLd0_dLeqFn-0PRSoFsYaRIFY8m7NI9fICN07dRvfK_VRsM-outJ5uJqA";
            string base64 = safe.Replace("-", "+").Replace("_", "/");



            try
            {
                Convert.FromBase64String(base64);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex);
            }
            
            //string content = System.Text.Encoding.UTF8.GetString(b);
            //Console.WriteLine(content);

            new System.Threading.ManualResetEvent(false).WaitOne();
        }
    }
}
