﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EvilDICOM.Core.Interfaces;
using EvilDICOM.Core.Dictionaries;
using EvilDICOM.Core.Enums;
using EvilDICOM.Core.Element;
using System.Reflection;
using EvilDICOM.Core.IO.Writing;

namespace EvilDICOM.Core.IO.Data
{
    public class DataComposer
    {
        public static byte[] GetDataLittleEndian(IDICOMElement el)
        {
            VR vr = VRDictionary.GetVRFromType(el);
            switch (vr)
            {
                case VR.AttributeTag:
                    AttributeTag at = el as AttributeTag;
                    return LittleEndianWriter.WriteTag(at.Data);
                case VR.FloatingPointDouble:
                    FloatingPointDouble fpd = el as FloatingPointDouble;
                    return LittleEndianWriter.WriteDoublePrecision(fpd.Data);
                case VR.FloatingPointSingle:
                    FloatingPointSingle fps = el as FloatingPointSingle;
                    return LittleEndianWriter.WriteSinglePrecision(fps.Data);
                case VR.OtherByteString:
                    OtherByteString obs = el as OtherByteString;
                    return DataRestriction.EnforceEvenLength(obs.Data.MultipicityValue.ToArray(), vr);
                case VR.OtherFloatString:
                    OtherFloatString ofs = el as OtherFloatString;
                    return ofs.Data.MultipicityValue.ToArray();
                case VR.OtherWordString:
                    OtherWordString ows = el as OtherWordString;
                    return ows.Data.MultipicityValue.ToArray();
                case VR.SignedLong:
                    SignedLong sl = el as SignedLong;
                    return LittleEndianWriter.WriteSignedLong(sl.Data);
                case VR.SignedShort:
                    SignedShort sis = el as SignedShort;
                    return LittleEndianWriter.WriteSignedShort(sis.Data);
                case VR.Unknown:
                    Unknown uk = el as Unknown;
                    return DataRestriction.EnforceEvenLength(uk.Data.MultipicityValue.ToArray(), vr);
                case VR.UnsignedLong:
                    UnsignedLong ul = el as UnsignedLong;
                    return LittleEndianWriter.WriteUnsignedLong(ul.Data);
                case VR.UnsignedShort:
                    UnsignedShort ush = el as UnsignedShort;
                    return LittleEndianWriter.WriteUnsignedShort(ush.Data);
                default: return GetStringBytes(vr, el);
            }
        }


        public static byte[] GetStringBytes(VR vr, IDICOMElement el)
        {
            string data;
            byte[] unpadded;
            switch (vr)
            {
                case VR.AgeString:
                    AgeString age = el as AgeString;
                    data = age.Data.SingleValue;
                    unpadded = GetASCIIBytes(data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.ApplicationEntity:
                    ApplicationEntity ae = el as ApplicationEntity;
                    unpadded = GetASCIIBytes(ae.Data.SingleValue);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.CodeString:
                    CodeString cs = el as CodeString;
                    unpadded = GetASCIIBytes(cs.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.Date:
                    Date d = el as Date;
                    data = StringDataComposer.ComposeDate(d.Data.SingleValue);
                    unpadded = GetASCIIBytes(data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.DateTime:
                    EvilDICOM.Core.Element.DateTime dt = el as EvilDICOM.Core.Element.DateTime;
                    data = StringDataComposer.ComposeDateTime(dt.Data.SingleValue);
                    unpadded = GetASCIIBytes(data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.DecimalString:
                    DecimalString ds = el as DecimalString;
                    data = StringDataComposer.ComposeDecimalString(ds.Data.MultipicityValue.ToArray());
                    unpadded = GetASCIIBytes(data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.IntegerString:
                    IntegerString iSt = el as IntegerString;
                    data = StringDataComposer.ComposeIntegerString(iSt.Data.MultipicityValue.ToArray());
                    unpadded = GetASCIIBytes(data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.LongString:
                    LongString ls = el as LongString;
                    unpadded = GetASCIIBytes(ls.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.LongText:
                    LongText lt = el as LongText;
                    unpadded = GetASCIIBytes(lt.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.PersonName:
                    PersonName pn = el as PersonName;
                    unpadded = GetASCIIBytes(pn.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.ShortString:
                    ShortString ss = el as ShortString;
                    unpadded = GetASCIIBytes(ss.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.ShortText:
                    ShortText st = el as ShortText;
                    unpadded = GetASCIIBytes(st.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.Time:
                    Time t = el as Time;
                    data = StringDataComposer.ComposeTime(t.Data.SingleValue);
                    unpadded = GetASCIIBytes(data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.UnlimitedText:
                    UnlimitedText ut = el as UnlimitedText;
                    unpadded = GetASCIIBytes(ut.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                case VR.UniqueIdentifier:
                    UniqueIdentifier ui = el as UniqueIdentifier;
                    unpadded = GetASCIIBytes(ui.Data);
                    return DataRestriction.EnforceEvenLength(unpadded, vr);
                default: return null;
            }
        }

        public static byte[] GetASCIIBytes(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return new ASCIIEncoding().GetBytes(s);
            }
            else
            {
                return new byte[0];
            }
        }
    }
}
