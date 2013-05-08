<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerupdmemberfi_positiontaking">
        <table class="tblDesc">
            <xsl:for-each select="info">
                <tr class="headerDetailNormal" >
                    <td colspan="6">{lbl:custInfo}</td>
                </tr>
                <tr >
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
                    <td class="colW1">{lbl:SubAccName}</td>
                    <td class="colW2">
                        <xsl:value-of select="@subaccname" />
                    </td>
                    <td class="colW1">{lbl:IP}</td>
                    <td class="colW2">
                        <xsl:value-of select="@ip" />
                    </td>
                    <td class="colW1">{lbl:Language}</td>
                    <td class="colW2">
                        <xsl:value-of select="@language" />
                    </td>
                </tr>
                <tr >
                   
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



            </xsl:for-each>
            <xsl:for-each select="ag">
                <tr class="headerDetailNormal" >
                    <td colspan="6">{lbl:ag}</td>
                </tr>
                <tr >
                    <td class="colW1">{lbl:v201apt_2}</td>
                    <td class="colW2">
                        <xsl:value-of select="@v201apt_2" />
                    </td>
                    <td class="colW1">{lbl:v201apt_3}</td>
                    <td class="colW2">
                        <xsl:value-of select="@v201apt_3" />
                    </td>
                    <td class="colW1"></td>
                    <td class="colW2">

                    </td>
                </tr>

            </xsl:for-each>

        </table>
    </xsl:template>



</xsl:stylesheet>
