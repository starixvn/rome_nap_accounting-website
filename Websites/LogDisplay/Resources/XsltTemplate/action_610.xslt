<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="age_custsubupdinfo">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1">{lbl:custID}</td>
                <td class="colW2">
                    <xsl:value-of select="@custid" />
                </td>-->
                <td class="colW1">{lbl:subAccName}</td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <td class="colW1">{lbl:fName}</td>
                <td class="colW2">
                    <xsl:value-of select="@fname" />
                </td>
            
                <td class="colW1">{lbl:lName}</td>
                <td class="colW2">
                    <xsl:value-of select="@lname" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:closed}</td>
                <td class="colW2">
                    <xsl:value-of select="@closed" />
                </td>
                <td class="colW1">{lbl:language}</td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>
                <!--<td class="colW1">{lbl:loginSubAccID}</td>
                <td class="colW2">
                    <xsl:value-of select="@loginsubaccid" />
                </td>-->
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:loginSubAccID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@loginsubaccid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
               
            </tr>
            <tr>
                <td class="colW1">{lbl:loginIP}</td>
                <td class="colW2">
                    <xsl:value-of select="@loginip" />
                </td>
                <!--<td class="colW1">{lbl:userID}</td>
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
          
                <td class="colW1">{lbl:errCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">{lbl:errMsg}</td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
          
            </tr>
        </table>





    </xsl:template>
</xsl:stylesheet>
