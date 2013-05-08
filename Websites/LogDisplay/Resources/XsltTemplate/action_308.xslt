<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdminmaxbetfi">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1">{lbl:custid}</td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->
                <td class="colW1">{lbl:MinBv201}</td>
                <td class="colW2">
                    <xsl:value-of select="@minbv201" />
                </td>
                <td class="colW1">{lbl:MaxBv201}</td>
                <td class="colW2">
                    <xsl:value-of select="@maxbv201" />
                </td>
                <td class="colW1">{lbl:MPMv201}</td>
                <td class="colW2">
                    <xsl:value-of select="@mpmv201" />
                </td>
            </tr>
            <tr>

                <td class="colW1">{lbl:SubAccName}</td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <!--<td class="colW1">{lbl:UserID}</td>
                <td class="colW2">
                    <xsl:value-of select="@userid" />
                </td>-->
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:userID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->

                <td class="colW1">{lbl:Language}</td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:ErrCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>

                <td class="colW1">{lbl:ErrMsg}</td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
                <td class="colW1"></td>
                <td class="colW2">

                </td>
            </tr>
        </table>
    </xsl:template>

</xsl:stylesheet>
