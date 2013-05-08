<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testsendemail.aspx.cs" Inherits="Test_testsendemail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style='width: 500px; border-collapse: collapse;
            font-family: Tahoma,Arial,helvetica,sans-serif; font-size: 11px; line-height: 18px;
            text-align: center'>
            <thead>
                <tr style='font-family: Tahoma; font-size: 8pt; font-weight: bold; text-transform: uppercase;
                    background-color: #B5C4D0 !important; border: 1px solid #7C98A4'>
                    <th nowrap style="border: 1px solid #7C98A4;">
                        CODE
                    </th>
                    <th style="border: 1px solid #7C98A4;">
                        MATCH EVENT
                    </th>
                    <th width='150' style="border: 1px solid #7C98A4;">
                        EVENT DATE
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr style='font-size: 11px; font-weight: bold; line-height: 18px'>
                    <td style="border: 1px solid #7C98A4;">
                        {MatchCode}
                    </td>
                    <td nowrap style="border: 1px solid #7C98A4;">
                        {Home}-vs{Away}
                    </td>
                    <td style="border: 1px solid #7C98A4;">
                        {EventDate}
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <br />

    <table cellspacing="0" style="border-collapse:collapse; width:930; font-family: Tahoma; font-size: 8pt; border: 1px solid #7C98A4; text-align:center">
                            <thead>
                            <tr style="height: 25px; background-color: #B5C4D0 !important;">
                                <th style="width: 30px; border: 1px solid #7C98A4">
                                    NO
                                </th>
                                <th style="width: 110px; border: 1px solid #7C98A4">
                                    ACCOUNT
                                </th>
                                <th style="width: 80px; border: 1px solid #7C98A4">
                                    REFNO
                                </th>
                                <th style="width: 80px; border: 1px solid #7C98A4">
                                    BET TYPE
                                </th>
                                <th style="width: 30px; border: 1px solid #7C98A4">
                                    BET
                                </th>
                                <th style="width: 40px; border: 1px solid #7C98A4">
                                    ODDS
                                </th>
                                <th style="width: 60px; border: 1px solid #7C98A4">
                                    AMOUNT
                                </th>
                                <th style="width: 70px; border: 1px solid #7C98A4">
                                    CURRENCY
                                </th>
                                <th style="width: 30px; border: 1px solid #7C98A4">
                                    HDP
                                </th>
                                <th style="width: 120px; border: 1px solid #7C98A4">
                                    TRANS.TIME
                                </th>
                                <th style="width: 40px; border: 1px solid #7C98A4">
                                    SCORE
                                </th>
                                <th style="width: 110px; border: 1px solid #7C98A4">
                                    CREATOR
                                </th>
                                <th style="width: 60px; border: 1px solid #7C98A4">
                                    STATUS
                                </th>                                     
                            </tr>
                            </thead>
                            <tr>
                                <td style="width: 30px; border: 1px solid #7C98A4">
                                    {Order}
                                </td>
                                <td style="width: 110px; border: 1px solid #7C98A4">
                                    {Account}
                                </td>
                                <td style="width: 80px; border: 1px solid #7C98A4">
                                    {TransId}
                                </td>
                                <td style="width: 80px; border: 1px solid #7C98A4">
                                    {Bettype}
                                </td>
                                <td style="width: 30px; border: 1px solid #7C98A4">
                                    {Bettype}
                                </td>
                                <td style="width: 40px; border: 1px solid #7C98A4">
                                    <b><font color="blue">{Odd}</font><div style="text-transform:uppercase; font-weight:bold">{OddType}</div></b>
                                </td>
                                <td style="text-align: right; width: 60px; border: 1px solid #7C98A4">
                                    <b><font color="red">{Stake}</font></b>
                                </td>
                                <td style="width: 70px; border: 1px solid #7C98A4">
                                    {Currency}
                                </td>
                                <td style="width: 30px; border: 1px solid #7C98A4">
                                    {Hdp}
                                </td>
                                <td style="width: 120px; border: 1px solid #7C98A4">
                                    {Transdate}
                                </td>
                                <td style="width: 40px; border: 1px solid #7C98A4">
                                    {Score}
                                </td>
                                <td style="width: 110px; border: 1px solid #7C98A4">
                                    {Creator}
                                </td>
                                <td style="width: 60px; border: 1px solid #7C98A4">
                                    {Status}
                                </td>
                            </tr>
                        
                        </table>

    </form>
</body>
</html>
