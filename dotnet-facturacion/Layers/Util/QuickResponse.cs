using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuickResponse
{
    public static Image QuickResponseGenerador(string input, int qrlevel)
    {

        string toenc = input;

        MessagingToolkit.QRCode.Codec.QRCodeEncoder qe = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();

        qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;

        qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; // - Using LOW for more storage

        qe.QRCodeVersion = qrlevel;

        System.Drawing.Bitmap bm = qe.Encode(toenc);

        return bm;

    }


}

